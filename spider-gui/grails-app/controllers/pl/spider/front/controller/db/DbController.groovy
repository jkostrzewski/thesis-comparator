package pl.spider.front.controller.db

@Grab(group='com.gmongo', module='gmongo', version='1.0')
import com.gmongo.GMongo

import grails.converters.JSON
import groovy.json.JsonBuilder

import org.codehaus.groovy.grails.web.json.JSONObject
import org.springframework.web.multipart.MultipartHttpServletRequest
import org.springframework.web.multipart.commons.CommonsMultipartFile

import pl.spider.front.groovy.utils.DataTablesBuilder
import pl.spider.front.service.db.DbService;
import pl.spider.front.service.search.SearchService;
import org.apache.commons.io.IOUtils;

class DbController {

	def dbService
	def searchService

	DataTablesBuilder dataTablesBuilder = new DataTablesBuilder()

	
	def uploadManualy = {
				
		def url = new URL(params.url)
		def urltosave = params.url
		HttpURLConnection connection = (HttpURLConnection) url.openConnection()
		def uuid = UUID.randomUUID().toString()
		def contentType = connection.getContentType()
			if (contentType.contains("html")) 
			{
				dbService.saveWEB(urltosave, uuid)
				dbService.saveWEBOrignal(urltosave, uuid)
				render 'success'
			}
			else 
			{
				dbService.saveURLRobot(urltosave, "")
				render 'success'
			}
	}
	

	

	def uploadLocalFile = {
		def f = request.getFile('file[]')
		println f
		assert f instanceof CommonsMultipartFile
		println f
		if (!f.empty) {
			if (dbService.saveFile(f)) {
				render 'success'
			} else {

				render 'error'
			}
		} else {
			render 'pusty'
		}
		render 'success'
	}




	def getData = {
			def resp = dataTablesBuilder.getData(dbService.getFilesList())
			if (searchService.searchEnded==true){
				resp.searchEnded = true
			}
			render resp as JSON
	}

	def deleteFile = {
		def fileId = params.aData
		dbService.deleteFile(fileId)
		render "success"
	}

	def getOriginalFile = {
		def id = params.id
		println id
		def file = dbService.retrieveFile(id)
		if (file != null) {
			response.setCharacterEncoding("UTF-8")
			response.outputStream << file.getInputStream()
		} else render "Check if you allowed to collect original files."
	}

	def getRawFile = {
		def id = params.id
		println id
		def file = dbService.retrieveRawFile(id)
		if (file != null) {
			def r =  IOUtils.toString(file.getInputStream(), 'UTF-8')
			
			
			String result = r.toLowerCase().replaceAll(/[\s]+/, ' ').replaceAll(/[ ]+/, ' ').replaceAll(/[^a-zA-Z0-9.,\-?! śćżółęąń]/, '').replaceAll(/[\\.]+/, ".").replaceAll(/[ ]{2,}/, ' ')
			//result.toCharArray().each{ch->
			//	printf("%04X%n", (int)ch);
			//	}
			println result
			response.setCharacterEncoding("UTF-8")
			response.setHeader("Access-Control-Allow-Origin","*")
			render result
		} else render "File not found"
	}
	
	def dropDB = { 
		println "Drop"
		dbService.clearDatabase()
		render "success"
	}
}
