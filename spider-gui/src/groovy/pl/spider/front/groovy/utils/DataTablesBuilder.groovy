package pl.spider.front.groovy.utils

import groovy.json.JsonBuilder
import java.text.DecimalFormat
import org.codehaus.groovy.grails.web.json.JSONObject

class DataTablesBuilder {
	def urlUtils = new UrlUtils()
	def getData(def dbData){ 
		DecimalFormat twoDForm = new DecimalFormat("#.##");
		def json = new JSONObject()
		def tmp = new JSONObject()
		def data = []
		for (i in dbData){
			tmp = new JSONObject()
			tmp.put("filename", i.filename)
			tmp.put("keywords", i.keywords)
			tmp.put("MD5", i.md5)
			tmp.put("fileId", i.fileId)
			tmp.put("uploadDate", i.uploadDate)
			tmp.put("orginalUrl", urlUtils.getBaseUrl(i.orginalUrl))
			tmp.put("length", twoDForm.format(new Float(i.length/1000)))
			
			data.add(tmp)
		}
		json.put('aaData', data)
		json.put('searchEnded', false)
		return json
	}
	
}
