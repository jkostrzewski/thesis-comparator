package pl.spider.front.groovy.utils

import org.apache.commons.lang.StringUtils;
import java.net.URL;

class UrlUtils {

	URLEncoder urlEncoder = new URLEncoder()

	def String normalizeUrl(def inHost, def inUrl){
		try {
			inUrl = StringUtils.removeStart(inUrl, "//")
			URI  uri = new URI(cleanUrl(inUrl))
			def scheme = uri.getScheme()?:'http'
			def host = uri.getHost()?:inHost
			def path = uri.getPath()?:null
			def query = uri.getQuery()?:null
			def fragment = null
			URI outUri = new URI(scheme, host, path, query, fragment)
			//System.out.println("Normalization: " +inUrl + " normalized to " + outUri.toString())
			return outUri.toString()
			
		}
		catch (Exception e) {
			System.out.println('Could not normalize url: '+ inUrl + e);

			return inUrl
		}
	}

	def String cleanUrl(String url){
		if (url && !url.isEmpty()){
			url = url.replaceAll("\\s","+")
			if (!StringUtils.startsWith(url, 'http://') && !StringUtils.startsWith(url, 'https://')){
				url = 'http://'+url
			}

			return url
		}
		return null
	}

	def String getBaseUrl(def inUrl){
		if (inUrl && !inUrl.isEmpty()){
			URL url = new URL(cleanUrl(inUrl))
			return url.getHost()
		}
		return null
	}
}
