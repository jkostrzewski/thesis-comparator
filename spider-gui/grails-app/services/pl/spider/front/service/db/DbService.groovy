package pl.spider.front.service.db


@Grab(group='com.gmongo', module='gmongo', version='1.0')


import com.gmongo.GMongo

import java.lang.String
import java.nio.charset.Charset;

import org.jsoup.Jsoup
import org.jsoup.Connection.Response
import org.jsoup.examples.HtmlToPlainText
import org.jsoup.nodes.Document

import com.mongodb.BasicDBObject
import com.mongodb.DBCursor
import com.mongodb.gridfs.GridFS
import com.mongodb.gridfs.GridFSDBFile
import com.sun.corba.se.spi.orbutil.fsm.MyFSM;

import pl.spider.front.java.web.rest.RestService
import grails.converters.JSON
import grails.converters.XML

import javax.annotation.PostConstruct;

import org.apache.tika.Tika
import org.apache.tika.metadata.Metadata
import org.codehaus.groovy.grails.io.support.IOUtils


class DbService {
	def mongo = new GMongo("localhost:27017")
	def gridfs = mongo.getDB("docs")
	def gridfsraw = mongo.getDB("docs_raw")
	def myfs = new GridFS(gridfs)
	def myfsraw = new GridFS(gridfsraw)





	//Metody dla recznego uploadu

	def saveURL(String url) {
		def tika = new Tika()
		def thisUrl = new URL(url)
		def newFileName = url.substring(url.lastIndexOf('/') + 1, url.length())
		def connection = thisUrl.openConnection()
		def fileInputStream = connection.inputStream
		def inputFile = myfs.createFile(fileInputStream)
		inputFile.setFilename(newFileName)
		//inputFile.put("orginalUrl",thisUrl)
		inputFile.put("fileId", UUID.randomUUID().toString())
		inputFile.save()


		Metadata metadata = new Metadata();
		metadata.add(Metadata.CONTENT_ENCODING, "UTF-8");
		def parsedString = tika.parseToString(thisUrl, metadata)
		println parsedString
		def InputStream webInputStream = new ByteArrayInputStream(parsedString.getBytes("UTF-8"))
		def inputFileTika = myfsraw.createFile(webInputStream)
		inputFileTika.put("fileId", UUID.randomUUID().toString())
		inputFileTika.setFilename(newFileName)
		//inputFileTika.put("orginalUrl",thisUrl)
		inputFileTika.save()

	}




	def saveWEB(String url, String uuid) {
		def thisUrl = url
		Document doc = Jsoup.connect(thisUrl).get()
		def newFileName = url.substring(url.lastIndexOf('/') + 1, url.length())
		String htmlString = doc.toString()
		def plainText = Jsoup.parse(htmlString,"utf-8").select("body").text()
		InputStream webInputStream = new ByteArrayInputStream(plainText.getBytes("UTF-8"))
		def inputFile = myfsraw.createFile(webInputStream)
		inputFile.setFilename(newFileName)
		inputFile.put("orginalUrl",thisUrl.toString())
		inputFile.put("fileId", uuid)
		inputFile.save()

	}

	def saveWEBOrignal(String url, String uuid) {
		def thisUrl = new URL(url)
		def newFileName = url.substring(url.lastIndexOf('/') + 1, url.length())
		def connection = thisUrl.openConnection()
		def fileInputStream = connection.inputStream
		def inputFile = myfs.createFile(fileInputStream)
		inputFile.setFilename(newFileName)
		inputFile.put("orginalUrl",thisUrl.toString())
		inputFile.put("fileId", uuid)
		inputFile.save()

	}

	//Metody dla Robota

	def saveWEBRobot(Document doc, String url, def keywords) {
		def thisUrl = url
		def newFileName = url.substring(url.lastIndexOf('/') + 1, url.length())
		String htmlString = doc.toString()
		
		def plainText = Jsoup.parse(htmlString).select("body").text()
		print plainText
		def uuid = UUID.randomUUID().toString()
		
		InputStream webInputStream = new ByteArrayInputStream(plainText.getBytes("UTF-8"))
		def inputFile = myfsraw.createFile(webInputStream)
		inputFile.setFilename(newFileName)
		inputFile.put("orginalUrl",url)
		inputFile.put("keywords", keywords)
		inputFile.put("fileId", uuid)
		inputFile.save()

		webInputStream = new ByteArrayInputStream(htmlString.getBytes("UTF-8"))
		inputFile = myfs.createFile(webInputStream)
		inputFile.setFilename(newFileName)
		inputFile.put("orginalUrl",url)
		inputFile.put("keywords", keywords)
		inputFile.put("fileId", uuid)
		inputFile.save()
	}



	def saveURLRobot(String url, def keywords) {
		try{
			def tika = new Tika()
			def thisUrl = new URL(url)
			def uuid = UUID.randomUUID().toString()
			def newFileName = url.substring(url.lastIndexOf('/') + 1, url.length())
			def connection = thisUrl.openConnection()
			def fileInputStream = connection.inputStream
			def inputFile = myfs.createFile(fileInputStream)
			inputFile.setFilename(newFileName)
			inputFile.put("orginalUrl",url)
			inputFile.put("keywords", keywords)
			inputFile.put("fileId", uuid)
			inputFile.save()

			Metadata metadata = new Metadata();
			metadata.add(Metadata.CONTENT_ENCODING, "UTF-8");
			def parsedString = tika.parseToString(thisUrl)
			def InputStream webInputStream = new ByteArrayInputStream(parsedString.getBytes("UTF-8"))
			def inputFileTika = myfsraw.createFile(webInputStream)
			inputFileTika.put("fileId", uuid)
			inputFileTika.setFilename(newFileName)
			inputFileTika.put("orginalUrl",url)
			inputFileTika.save()
		}catch(Exception e){

		}

	}


	boolean saveFile(file) {
		def contentType = file.getContentType()
		def filename = file.getOriginalFilename()

		try {
			if (myfs.findOne(filename) == null) {
				saveLocalFile(file, contentType, filename)
			} else {
				println "Removing old file and uploading new file"
				myfs.remove(filename)
				myfsraw.remove(filename)
				saveLocalFile(file, contentType, filename)
			}
		} catch (Exception ex) {
			throw ex
		}
		return true
	}

	def saveLocalFile(def file, def contentType, def filename) {
		def inputFile = myfs.createFile(file.getInputStream())
		def uuid = UUID.randomUUID().toString()
		inputFile.setContentType(contentType)
		inputFile.setFilename(filename)
		inputFile.put("fileId", uuid)
		inputFile.save()
		["pdf", "doc", "docx", "html", "htm"].each() {i->
			if (contentType.contains(i)){
				def tika = new Tika()
				Metadata metadata = new Metadata();
				metadata.add(Metadata.CONTENT_ENCODING, "UTF-8");
				def parsedString = tika.parseToString(file.getInputStream(), metadata)
				def InputStream fileStream = new ByteArrayInputStream(parsedString.getBytes("UTF-8"))
				def inputFileTika = myfsraw.createFile(fileStream)
				inputFileTika.put("fileId", uuid)
				inputFileTika.setFilename(filename)
				inputFileTika.put("orginalUrl","local disc")
				inputFileTika.save()
			}
		}
		['text/plain', 'txt'].each() {i->
		if (contentType.contains([i])){
			print file
			def plain = file.getInputStream().getText("UTF-8")
			print plain
			
			def InputStream fileStream = new ByteArrayInputStream(plain.getBytes("UTF-8"))
			def input = myfsraw.createFile(fileStream)
			input.put("fileId", uuid)
			input.setFilename(filename)
			input.put("orginalUrl","local disc")
			input.save()
		}
		}
		

	}

	def retrieveFile(String fileId) {
		BasicDBObject query = new BasicDBObject("fileId", fileId)
		return myfs.findOne(query)
	}

	def retrieveRawFile(String fileId) {
		BasicDBObject query = new BasicDBObject("fileId", fileId)
		return myfsraw.findOne(query)
	}


	def deleteFile(def fileId) {
		BasicDBObject query = new BasicDBObject("fileId", fileId)
		def cursor = myfs.find(query)
		myfs.remove(cursor)

	}

	def getFilesList() {
		def cursor1 = myfs.getFileList()
		def converter = cursor1.toArray()
		return converter
	}


	def clearDatabase(){
		gridfs.dropDatabase()
		gridfsraw.dropDatabase()
	}
	
	def String byteToString(ByteArrayInputStream inputReader){
		if(inputReader == null){
			return null;
		}
		 inputReader.reset();
		 int c;
		 StringWriter sWriter = new StringWriter()
		 while((c=inputReader.read())!=-1){
			 sWriter.write(c);
		 }
		 inputReader.reset();
		 def r = sWriter.toString()
		 return r;
   }

}
