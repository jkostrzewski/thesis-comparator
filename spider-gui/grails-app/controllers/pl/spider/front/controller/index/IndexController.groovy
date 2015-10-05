package pl.spider.front.controller.index

class IndexController {
	
	def index() {
		render (view:"../pages/index.gsp")
	}

	def init(){
		System.setProperty("file.encoding","UTF-8")
		println System.getProperty("file.encoding")
		if (params.keywords){
			def keywords = params.keywords.tokenize(',')
			session.keywords = keywords
		}else{
		session.keywords = null
		}
		session.searchEnded = false

		if (params.autostart==1.toString()){
		render (view: "../layouts/main.gsp", model:[autostart:1])
		}else{
		render (view: "../layouts/main.gsp", model:[autostart:0])
		}
	}

	def show(){
		render template:'index'
	}
}
