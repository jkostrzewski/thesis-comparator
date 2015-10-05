package pl.spider.front.service.search

import org.jsoup.Connection.Response
import org.jsoup.nodes.Document
import org.jsoup.Jsoup

import pl.spider.front.groovy.utils.UrlUtils
import pl.spider.front.java.model.FileTypes.FileType;
import pl.spider.front.java.model.Page
import pl.spider.front.java.model.RobotStates.RobotState
import pl.spider.front.java.model.repository.Repository
import pl.spider.front.java.web.rest.RestService
import pl.spider.front.service.db.DbService

class Robot implements Runnable {

	RestService restService = new RestService()
	DbService dbService = new DbService()
	private int robotId;
	private Repository repository;
	RobotState robotState
	UrlUtils urlUtils = new UrlUtils()
	def terminate = false

	public Robot(Repository repository, int id) {
		this.repository = repository;
		this.robotId = id;
		this.robotState = RobotState.RUNNING;
		this.terminate = false;
	}

	@Override
	public void run() {
		boolean searchEnded = false;
		while (!searchEnded){
			if (terminate){
				searchEnded = true
			}
			switch (robotState) {
				case RobotState.PENDING:
					try {
						Thread.sleep(500);
						if (!repository.isSeedListEmpty()) {
							robotState = RobotState.RUNNING;
						}
					} catch (InterruptedException e) {
						e.printStackTrace();
					}
					break;

				case RobotState.RUNNING:
					Page seedPage;
					if (((seedPage = repository.getFromSeedList()) != null) && (repository.incCounter())) {

						System.out.println("Robot:" + robotId + " site: "

								+ seedPage.getUrl() + " on depth "
								+ seedPage.getDepth() + " seed size "+ repository.getListSize());

						Response response = restService.doGet(seedPage.getUrl());
						if (response != null){
							processResponse(response, seedPage)
						}
					} else {
						robotState = RobotState.PENDING;
					}

					break;

				case RobotState.FINISHED:
					searchEnded=true;
					println "Robot "+ robotId+" finished."
					break;
			}
		}
	}

	boolean isRobotWorking(){
		return (robotState==RobotState.RUNNING)
	}
	public void setFinishState(){
		this.robotState=RobotState.FINISHED
	}

	def processResponse(Response response, def seedPage){
		if (response==null){
			return null
		}
		Document doc = Jsoup.parse(response.body(), "UTF-8")
		def keywords = repository.getProperties().getKeywords()
		def contentType = response.contentType()
		def acceptedTypes = repository.getProperties().getAcceptedTypes()
		if (contentType.contains("html")) {
			for (i in acceptedTypes){
				if (contentType.contains(i.toString()) || i==FileType.all){
					dbService.saveWEBRobot(doc,seedPage.getUrl(), keywords);
				}
			}
			repository.addAllToSeedList(restService.getAnchors(doc, true), seedPage.getDepth() - 1, urlUtils.getBaseUrl(seedPage.getUrl()))
		}
		else{
			for (i in acceptedTypes){
				if (contentType.contains(i.toString()) || i == FileType.all){
					dbService.saveURLRobot(seedPage.getUrl().toString(), keywords);
				}
			}
		}
	}
}


