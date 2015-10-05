package pl.spider.front.service.search

import java.sql.Connection;
import java.util.concurrent.ExecutorService

import org.jsoup.Connection.Response

import pl.spider.front.java.model.FileTypes;
import pl.spider.front.java.model.FileTypes.FileType
import pl.spider.front.java.model.Priorities.Priority
import pl.spider.front.java.model.repository.Repository
import pl.spider.front.java.model.repository.RepositoryProperties
import pl.spider.front.java.web.rest.RestService

import java.util.concurrent.ExecutorService
import java.util.concurrent.Executors
import java.util.concurrent.Callable
import java.util.concurrent.Executors
import java.util.concurrent.TimeUnit
import java.util.concurrent.Future
import org.codehaus.groovy.grails.web.util.WebUtils
import pl.spider.front.groovy.utils.TypeUtils
import pl.spider.front.groovy.utils.UrlUtils
import javax.servlet.http.HttpSession
import org.springframework.web.context.request.RequestContextHolder

class SearchService {

	RestService restService = new RestService()
	UrlUtils urlUtils = new UrlUtils()
	def robotService
	def dbService
	def repositoryService
	def searchEnded
	def grailsApplication
	def terminateSearch
	def repoEndedList = []
	def systemRobotReferences = []
	def threadReferences = []

	def search(def param){
		searchEnded = false
		terminateSearch = false
		def repositories
		if (param){
			repositories = prepareCustomRepositories(param)
		}else{
			repositories = prepareDefaultRepositories()
		}
		runSearch(repositories)
	}

	def runSearch(def repositories){

		repoEndedList = []
		systemRobotReferences = []
		threadReferences = []
		try{
			callAsync {
				repositories.each{Repository repo->
					repoEndedList.add(false)
					def noThreads = repo.getProperties().getNoThreads()
					def repoRobotReferences = []
					for (int j=0; j<noThreads; j++) {
						Robot robot = new Robot(repo, j)
						repoRobotReferences.add(robot)
						Thread t = new Thread(robot)
						threadReferences.add(t)
						t.start()
					}
					systemRobotReferences.add(repoRobotReferences)
				}
				while (!isSearchEnded(repoEndedList) && !terminateSearch){

					sleep(2000)
					systemRobotReferences.eachWithIndex{systemTabEntry, repoEntry->
						def toRemove = true
						systemTabEntry.each{Robot robot->
							if (robot.isRobotWorking()){
								toRemove = false
							}
						}
						if (toRemove){
							systemTabEntry.each{Robot robot->
								robot.setFinishState();
							}
							repoEndedList[repoEntry]=true
						}
					}
				}
				searchEnded = true
				def stop = new Date().getTime()

				println "skonczlismy"
		
			}
		}finally{
			systemRobotReferences.eachWithIndex{systemTabEntry, repoEntry->
				systemTabEntry.each{Robot robot->
					robot.terminate = true
				}
				repoEndedList[repoEntry]=true
			}
			threadReferences.each{Thread thread->
				thread.interrupt();
			}
		}
	}





	def isSearchEnded(def repoEndedList){
		println repoEndedList.toString()
		return !repoEndedList.any {it==false}
	}

	def prepareCustomRepositories(def param){
		def repoCount = param."customizeCount"
		def repositories = []
		RepositoryProperties properties
		(0..repoCount).each{ i ->
			if (param."name-${i}"){
				def keywords = param."keywords-${i}"?.tokenize(',')
				def repositoryUrls = TypeUtils.toList(repositoryService.getUrls(param, i))
				def fileTypes = (TypeUtils.toList(param."accepted-types-${i}")).collect{ "${it}" as FileType }
				properties = new RepositoryProperties()
				properties.setStartUrl((ArrayList<String>)TypeUtils.toList(repositoryUrls))
				properties.setBaseUrl(urlUtils.getBaseUrl(param."url-${i}"))
				properties.setMaxDepth(Integer.valueOf(param."depth-${i}"))
				properties.setNoThreads(Integer.valueOf(param."threads-${i}"))
				properties.setName(param."name-${i}")
				properties.setKeywords(keywords)
				properties.setLanguages(param."languages-${i}" as List)
				properties.setMaxPages(param."max-pages-${i}"?Integer.valueOf(param."max-pages-${i}"):-1)
				properties.setAcceptedTypes(fileTypes?fileTypes:FileType.all as List)
				properties.setSaveOriginal(param."save-original-${i}" as boolean)

				repositories.add(new Repository(properties))
			}
		}
		return repositories
	}
	def prepareDefaultRepositories(){
		def repositories = []
		RepositoryProperties properties
		def session = RequestContextHolder.currentRequestAttributes().getSession()
		def keywords = session.keywords
		if (!keywords || keywords.isEmpty()){
			searchEnded = true
			return null
		}

		properties = new RepositoryProperties()
		properties.setMaxDepth(grailsApplication.config.defaultRepoSettings.depth as int)
		properties.setNoThreads(grailsApplication.config.defaultRepoSettings.noThreads as int)
		properties.setMaxPages(grailsApplication.config.defaultRepoSettings.maxPages as int)
		properties.setKeywords(keywords)
		properties.setAcceptedTypes(FileType.all as List)
		properties.setSaveOriginal(grailsApplication.config.defaultRepoSettings.saveOriginal as boolean)

		properties.setName("Bing")
		properties.setStartUrl((ArrayList<String>)TypeUtils.toList(repositoryService.getBingRepositories(keywords, null, null)))
		properties.setBaseUrl("http://www.bing.com/")
		repositories.add(new Repository(properties))

		properties.setName("Wikipedia")
		properties.setStartUrl((ArrayList<String>)TypeUtils.toList(repositoryService.getWikiRepositories(keywords, ["pl", "en"])))
		repositories.add(new Repository(properties))


		return repositories
	}

	def terminateSearch() {
		searchEnded = true
		terminateSearch = true
		
		systemRobotReferences.eachWithIndex{systemTabEntry, repoEntry->
			systemTabEntry.each{Robot robot->
				robot.terminate = true
			}
			repoEndedList[repoEntry]=true
		}
	
	}
}
