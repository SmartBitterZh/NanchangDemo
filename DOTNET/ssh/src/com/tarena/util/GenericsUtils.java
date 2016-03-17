package com.tarena.util;

import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;

@SuppressWarnings("unchecked")
public class GenericsUtils {
   
	/** 获取运行时类Class<?> */
	public static Class getGenericClass(Class clazz) {
        return getGenericClass(clazz, 0);
    }

    public static Class getGenericClass(Class clazz, int index) {
        Type genType = clazz.getGenericSuperclass();
        if (genType instanceof ParameterizedType) {
            Type[] params = ((ParameterizedType) genType).getActualTypeArguments();
            if ((params != null) && (params.length >= (index-1))) {
                return (Class) params[index];
            }
        }
        return null;
    }
}
