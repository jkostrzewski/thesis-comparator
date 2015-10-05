package pl.front.controller.search

class SearchController {
	
	def searchService
	
	def customizeSearch(){
		searchService.search(params)
		redirect (controller:'result', action:'show')
	}
	
	def defaultSearch(){
		searchService.search(null)
		redirect (controller:'result', action:'show')
	}
	
	def terminateSearch = {
		searchService.terminateSearch()
		render "success"
	}
}
