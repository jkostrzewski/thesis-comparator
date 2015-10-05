package pl.spider.front.java.model.repository;

import java.util.ArrayList;
import java.util.HashSet;

import pl.spider.front.groovy.utils.UrlUtils;
import pl.spider.front.java.model.Page;

public class Repository {

	private UrlUtils urlUtils = new UrlUtils();
	private RepositoryProperties properties;
	private ArrayList<Page> seedList;
	private HashSet<String> visitedSet;

	public Repository(RepositoryProperties properties) {
		this.properties = properties;
		seedList = new ArrayList<Page>();
		visitedSet = new HashSet<String>();
		addAllToSeedList(properties.getStartUrl(),
				properties.getMaxDepth(), properties.getBaseUrl());
	}

	public synchronized void addToSeedList(Page page) {

		page.setUrl(urlUtils.normalizeUrl(page.getBaseUrl(),
				page.getUrl()));
		if ((page.getDepth() > 0) && (!isUrlVisited(page.getUrl()))) {
			seedList.add(page);

		}
	}

	public synchronized Page getFromSeedList() {
		if (!seedList.isEmpty()) {
			Page page = seedList.get(0);
			seedList.remove(0);
			return page;
		}
		return null;
	}

	private boolean isUrlVisited(String url) {
		if (visitedSet.contains(url)) {
			return true;
		} else {
			addToVisited(url);
			return false;
		}
	}

	private void addToVisited(String url) {
		visitedSet.add(url);
	}

	public synchronized boolean isSeedListEmpty() {
		return seedList.isEmpty();
	}

	public synchronized void addAllToSeedList(ArrayList<String> list, int depth, String baseUrl) {
		if (list!=null){
		for (int i = 0; i < list.size(); i++) {
			addToSeedList(new Page(list.get(i), depth, baseUrl));
		}
		}
	}
	
	public synchronized int getListSize(){
		return seedList.size();
	}

	public synchronized boolean incCounter(){
		int counter = properties.getMaxPages();
		if (counter>0){
			properties.setMaxPages(counter-1);
			return true;
		} else if (counter<0){
			return true;
		}
		else if (counter==0){
			return false;
		}
		return false;
	}

	public RepositoryProperties getProperties() {
		return properties;
	}

	public void setProperties(RepositoryProperties properties) {
		this.properties = properties;
	}
}
