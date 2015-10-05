package pl.spider.front.java.model;

public class Page {

	private String url;
	private String baseUrl;
	private int depth;
	
	public Page(String url, int depth, String baseUrl){
		this.url = url;
		this.depth = depth;
		this.baseUrl = baseUrl;
	}

	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
	}

	public int getDepth() {
		return depth;
	}

	public void setDepth(int depth) {
		this.depth = depth;
	}

	public String getBaseUrl() {
		return baseUrl;
	}

	public void setBaseUrl(String baseUrl) {
		this.baseUrl = baseUrl;
	}
	

}
