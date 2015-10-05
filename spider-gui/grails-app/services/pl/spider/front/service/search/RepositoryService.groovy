package pl.spider.front.service.search

import grails.gsp.PageRenderer
import pl.spider.front.groovy.utils.TypeUtils
import pl.spider.front.groovy.utils.UrlUtils

class RepositoryService {
	
	PageRenderer groovyPageRenderer
	UrlUtils urlUtils = new UrlUtils()
	
	def getUrls(def param, def num){
		def name = param."name-${num}"
		def url = param."url-${num}"
		def keywords = TypeUtils.toList(param."keywords-${num}"?.tokenize(','))
		def fileTypes = TypeUtils.toList(param."accepted-types-${num}")
		def languages = TypeUtils.toList(param."languages-${num}")
		
		
		
		if (name.equals("bing")){
			return getBingRepositories(keywords, fileTypes - ["image"], languages)
		}else if(name.equals("Wikipedia")){
			return getWikiRepositories(keywords, languages)
		} else{
		return url
		}
	}
	
	def getGoogleRepositories(def keywords, def param){
		
	}

	def getBingRepositories(def keywords, def fileTypes, def languages){
		def urls = []
		def combinations = [keywords, languages, fileTypes].findResults { it ?: [null] }.combinations()
		combinations.each{ c->
			def url = groovyPageRenderer.render(template:'../urlTemplates/bingUrl', model:[keyword:URLEncoder.encode(c.getAt(0), "UTF-8"), language:c.getAt(1), fileType:c.getAt(2)])
			urls.add(urlUtils.cleanUrl(url))
		}
		println "urls "+ urls
		return urls
	}
	
	def getWikiRepositories(def keywords, def languages){
		def urls = []
		def combinations = [keywords, languages].findResults { it ?: [null] }.combinations()
		combinations.each{ c->
			def url = groovyPageRenderer.render(template:'../urlTemplates/wikiUrl', model:[keyword:URLEncoder.encode(c.getAt(0), "UTF-8"), language:c.getAt(1)])
			urls.add(urlUtils.cleanUrl(url))
		}
		println "urls "+ urls
		return urls
	}
}
