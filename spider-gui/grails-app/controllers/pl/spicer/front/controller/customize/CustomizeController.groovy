package pl.spicer.front.controller.customize

import grails.converters.JSON

class CustomizeController {
	
	def searchService
	def dictionaryService
	
	def show(){
		def repos =  dictionaryService.getLocalRepositories()
		def dict = dictionaryService.getDictionaries()
		def keywords = session.keywords?.join(', ')
		render (template:'customize', model:[repositories:repos.items, dictionary:dict, keywords:keywords])
	}
	
	def addRepository(){
		def name = params.name
		def url = params.url
		def id = Integer.valueOf(params.count)
		def result = [:]
		def dict = dictionaryService.getDictionaries()

		result.put('navEntry', g.render( template: 'navEntry', model:[name:name, id:id]))
		result.put('contentEntry', g.render( template: 'contentEntry', model:[name:name, url:url, id:id, dictionary:dict]))
		result.put('count', id+1)
		render result as JSON
	}
	


}
