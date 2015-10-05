package pl.spider.front.service.dictionary

import groovy.json.JsonSlurper;


class DictionaryService {
	
	def utilsService
	JsonSlurper slurper = new JsonSlurper()

	def getDictionaries(){
		def dictionaries = [:]
		dictionaries.put("acceptedTypes", utilsService.getLocalDictionary("accepted_types"))
		dictionaries.put("languages", utilsService.getLocalDictionary("languages"))
		
		return dictionaries
	}
	
	def getLocalRepositories(){
		def repositories = utilsService.getLocalDictionary("repositories")
		return repositories
	}

}
