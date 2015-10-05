package pl.spider.front.java.web.url;

public class UrlUtils {
	
	public String normalizeUrl(String baseUrl, String url){
		/*url = url.toLowerCase();
		if (!isSameSite(baseUrl, url))
			return null;
		*/
		return url;
	}
	
	private boolean isSameSite(String baseUrl, String url){
		return true;
	}
	
}

