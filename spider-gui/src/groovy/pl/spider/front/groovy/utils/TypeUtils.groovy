package pl.spider.front.groovy.utils

class TypeUtils {

	public static def toList(value) {
		value ?: []
	}

	public static def toList(String value){
		return [value]
	}
	
}
