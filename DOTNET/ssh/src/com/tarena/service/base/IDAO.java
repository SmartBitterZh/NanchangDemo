package com.tarena.service.base;

import java.io.Serializable;
import java.util.List;
import java.util.Map;

import com.tarena.util.QueryResult;

public interface IDAO<T> {

	/**
	 *  保存实体对象
	 * @param o 实体对象
	 */
	public void save(Object o);
	
	/**
	 * 更新实体对象
	 * @param o 实体对象
	 */
	public void update(Object o);
	
	/**
	 * 删除实体对象
	 * @param id 实体对象的id
	 */
	public void delete(Serializable id);
	
	/**
	 * 删除实体对象
	 * @param ids 实体对象的id数组
	 */
	public void delete(Serializable[] ids);
	
	/**
	 * 查找实体对象
	 * @param id 实体对象的id
	 * @return 实体对象
	 */
	public T find(Serializable id);
	
	/**
	 * 按属性查找实体对象
	 * @param property 属性名称
	 * @param value 属性值
	 * @return 符合条件的实体对象集合
	 */
	public List<T> findByProperty(String property,String value);
	
	/**
	 * 获取分页数据
	 * @param 数据的类型
	 * @param firstResult 开始索引
	 * @param maxResults 需要获取的记录数
	 * @return
	 */
	public QueryResult<T> getPageData(int firstResult,int maxResults,
			String wherehql,Object[] params,Map<String,String> orderby);
	
	public QueryResult<T> getPageData(int firstResult,int maxResults,
			String wherehql,Object[] params);
	
	public QueryResult<T> getPageData(int firstResult,int maxResults,
			Map<String,String> orderby);
	
	public QueryResult<T> getPageData(int firstResult,int maxResults);
	
	public QueryResult<T> getPageData();
}
