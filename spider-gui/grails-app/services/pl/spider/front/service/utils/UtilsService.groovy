package pl.spider.front.service.utils

import grails.converters.JSON

class UtilsService {
	
	def getLocalDictionary(String dictionaryName) {
		InputStream indexFileStream = this.class.classLoader.getResourceAsStream("robotConfig/"+ dictionaryName + ".txt")
		assert indexFileStream != null
		def json = JSON.parse(indexFileStream, "UTF-8")	
		json
	}

}
