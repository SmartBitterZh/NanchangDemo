package com.tarena.service.base;

import java.io.Serializable;
import java.util.List;
import java.util.Map;

import javax.annotation.Resource;

import org.hibernate.Query;
import org.hibernate.Session;
import org.springframework.orm.hibernate3.HibernateTemplate;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.tarena.util.GenericsUtils;
import com.tarena.util.QueryResult;

@SuppressWarnings("unchecked")
@Transactional
public abstract class AbstractDAO<T> implements IDAO<T> {
	
	@Resource(name="hibernateTemplate")
	protected HibernateTemplate hibernateTemplate;
	
	protected Class<T> clazz = GenericsUtils.getGenericClass(getClass());
	
	public void delete(Serializable id) {
		if(id != null)
			delete(new Serializable[]{id});
	}

	public void delete(Serializable[] ids) {
		if(ids != null && ids.length >0){
			for (Serializable id : ids) {
				hibernateTemplate.delete(hibernateTemplate.get(clazz, id));
			}
		}
	}

	@Transactional(readOnly=true,propagation=Propagation.NOT_SUPPORTED)
	public T find(Serializable id) {
		T t = null;
		if(id != null){
			t = (T)hibernateTemplate.get(clazz, id);
		}
		return t;
	}

	@Transactional(readOnly=true,propagation=Propagation.NOT_SUPPORTED)
	public List<T> findByProperty(String property,String value){
		String hql = "from " + getEntityName() + " o where o." + property + "=?";
		return hibernateTemplate.find(hql, value);
	}
	
	public void save(Object o) {
		if(o != null){
			hibernateTemplate.save(o);
		}
	}

	public void update(Object o) {
		if(o != null){
			hibernateTemplate.update(o);
		}
	}

	@Transactional(readOnly=true,propagation=Propagation.NOT_SUPPORTED)
	public QueryResult<T> getPageData(int firstResult,
			int maxResults, Map<String, String> orderby) {
		return getPageData(firstResult, maxResults, null,null, orderby);
	}

	@Transactional(readOnly=true,propagation=Propagation.NOT_SUPPORTED)
	public QueryResult<T> getPageData(int firstResult,
			int maxResults, String wherehql, Object[] params) {
		return getPageData(firstResult, maxResults, wherehql,params, null);
	}

	@Transactional(readOnly=true,propagation=Propagation.NOT_SUPPORTED)
	public QueryResult<T> getPageData(int firstResult,int maxResults) {
		return getPageData(firstResult, maxResults, null,null, null);
	}

	@Transactional(readOnly=true,propagation=Propagation.NOT_SUPPORTED)
	public QueryResult<T> getPageData() {
		return getPageData(-1, -1, null,null, null);
	}

	@Transactional(readOnly=true,propagation=Propagation.NOT_SUPPORTED)
	public QueryResult<T> getPageData(int firstResult,int maxResults,
			String wherehql,Object[] params,Map<String,String> orderby) {
		String entityName = getEntityName();
		QueryResult<T> qr = new QueryResult<T>();
		Session session = hibernateTemplate.getSessionFactory().getCurrentSession();
		Query query = session.createQuery("select o from " + entityName + " o " + 
				(wherehql==null?"": "where " + wherehql) + buildOrderby(orderby));
		setQueryParams(query, params);
		if(firstResult!=-1 && maxResults!=-1)
			query.setFirstResult(firstResult).setMaxResults(maxResults);
		qr.setResultList(query.list());
		query = session.createQuery("select count(o) from " + entityName + " o " + (wherehql==null?"": "where " + wherehql));
		setQueryParams(query, params);
		Object total = query.uniqueResult();
		if(total instanceof Integer)
			qr.setTotal((Integer)total);
		else if(total instanceof Long)
			qr.setTotal(((Long)total).intValue());
		return qr;
	}
	
	/**
	 * 设置条件语句的参数值
	 * @param query
	 * @param params
	 */
	protected void setQueryParams(Query query,Object[] params){
		if(params != null && params.length > 0){
			for(int i=0;i<params.length;i++){
				query.setParameter(i, params[i]);
			}
		}
	}
	
	/**
	 * 构造排序语句
	 * @param orderby
	 * @return
	 */
	protected String buildOrderby(Map<String,String> orderby){
		StringBuffer sb = new StringBuffer();
		if(orderby != null && orderby.size() > 0){
			sb.append(" order by ");
			for (String key : orderby.keySet()) {
				sb.append("o.").append(key).append(" ").append(orderby.get(key)).append(",");
			}
			sb.deleteCharAt(sb.length()-1);
		}
		return sb.toString();
	}
	
	/**
	 * 获取实体名称
	 * @return 实体名称
	 */
	protected String getEntityName(){
		return clazz.getSimpleName();
	}
}
