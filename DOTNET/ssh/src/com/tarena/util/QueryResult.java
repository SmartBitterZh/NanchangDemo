package com.tarena.util;

import java.util.List;

/** 用于封装查询结果 */
public class QueryResult<T> {

	/** 结果集 */
	private List<T> resultList;
	
	/** 结果总数 */
	private int total;

	public List<T> getResultList() {
		return resultList;
	}

	public void setResultList(List<T> resultList) {
		this.resultList = resultList;
	}

	public int getTotal() {
		return total;
	}

	public void setTotal(int total) {
		this.total = total;
	}
}
