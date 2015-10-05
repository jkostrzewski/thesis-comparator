package pl.spider.front.java.model.repository;

import java.util.ArrayList;
import java.util.List;

import pl.spider.front.java.model.FileTypes.FileType;
import pl.spider.front.java.model.Priorities.Priority;


public class RepositoryProperties {

	private String name;


	private ArrayList<String> startUrl;
	private String baseUrl;
	private List<String> keywords;
	private List<FileType> acceptedTypes;
	private List<String> languages;
	
	

	private Priority priority;
	
	private int noThreads;
	private int maxDepth;
	private int maxPages;
	
	private boolean saveOriginal;
	
	public RepositoryProperties(){
		keywords = new ArrayList<String>();
		acceptedTypes = new ArrayList<FileType>();
		startUrl = new ArrayList<String>();
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public ArrayList<String> getStartUrl() {
		return startUrl;
	}

	public void setStartUrl(String startUrl) {
		this.startUrl.add(startUrl);
	}
	public void setStartUrl(ArrayList<String> startUrl) {
		this.startUrl = startUrl;
	}

	public String getBaseUrl() {
		return baseUrl;
	}

	public void setBaseUrl(String baseUrl) {
		this.baseUrl = baseUrl;
	}

	public List<String> getKeywords() {
		return keywords;
	}

	public void setKeywords(List<String> keywords) {
		this.keywords = keywords;
	}

	public List<FileType> getAcceptedTypes() {
		return acceptedTypes;
	}

	public void setAcceptedTypes(List<FileType> acceptedTypes) {
		this.acceptedTypes = acceptedTypes;
	}

	public List<String> getLanguages() {
		return languages;
	}

	public void setLanguages(List<String> languages) {
		this.languages = languages;
	}

	public Priority getPriority() {
		return priority;
	}

	public void setPriority(Priority priority) {
		this.priority = priority;
	}

	public int getNoThreads() {
		return noThreads;
	}

	public void setNoThreads(int noThreads) {
		this.noThreads = noThreads;
	}

	public int getMaxDepth() {
		return maxDepth;
	}

	public void setMaxDepth(int maxDepth) {
		this.maxDepth = maxDepth;
	}

	public int getMaxPages() {
		return maxPages;
	}

	public void setMaxPages(int maxPages) {
		this.maxPages = maxPages;
	}

	public boolean isSaveOriginal() {
		return saveOriginal;
	}

	public void setSaveOriginal(boolean saveOriginal) {
		this.saveOriginal = saveOriginal;
	}
	

}
