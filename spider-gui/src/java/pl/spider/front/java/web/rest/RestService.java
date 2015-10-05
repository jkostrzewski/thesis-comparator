package pl.spider.front.java.web.rest;

import java.util.ArrayList;

import org.jsoup.Connection;
import org.jsoup.Connection.Response;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import org.jsoup.nodes.Element;
import org.jsoup.select.Elements;

public class RestService {

	int timeoutSecond = 1000;

	public Response doGet(String url) {
		try {
			Connection.Response response = Jsoup
					.connect(url)
					.ignoreContentType(true)
					.userAgent(
							"Mozilla/5.0 (Windows; U; WindowsNT 5.1; en-US; rv1.8.1.6) Gecko/20070725 Firefox/2.0.0.6")
					.timeout(10 * timeoutSecond).followRedirects(true)
					.execute();
			// System.out.println(response.parse().toString());
			if (response.statusCode() == 200) {
				return response;
			}
		} catch (Exception e) {
			System.out.println("Problem connecting to " + url);
		}
		return null;
	}

	public ArrayList<String> getAnchors(Document htmlPage, boolean getImages) {
		if (htmlPage != null) {
			ArrayList<String> result = new ArrayList<String>();
			Elements links = htmlPage.select("a[href]");
			for (Element link : links) {
				result.add(link.attr("href"));
			}
			if (getImages) {
				links = htmlPage.select("img[src]");
				for (Element link : links) {
					result.add(link.attr("src"));
				}
			}
			return result;
		}

		return null;
	}

}