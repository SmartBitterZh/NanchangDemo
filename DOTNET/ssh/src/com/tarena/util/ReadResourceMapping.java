package com.tarena.util;

import java.io.IOException;
import java.util.Properties;

/** 通过key从属性文件中读取相应的资源映射 */
public class ReadResourceMapping {
	
	private static Properties prop = new Properties();
	
	static{
		try {
			prop.load(ReadResourceMapping.class.getClassLoader().getResourceAsStream("myResourceMapping.properties"));
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	public static String get(String key){
		return (String)prop.get(key);
	}
}
