package com.tarena.formbean;

import org.apache.struts.action.ActionForm;

@SuppressWarnings("serial")
public class BaseForm extends ActionForm {

	/** 页码 */
	private int page;
	/** 每页最大记录数 */
	public int maxResult;
	/** 查询标志 */
	public String query;
	
	public int getPage() {
		return page < 1 ? 1 : page;
	}
	public void setPage(int page) {
		this.page = page;
	}

	public int getMaxResult() {
		return maxResult < 1 ? 10 : maxResult;
	}
	public void setMaxResult(int maxResult) {
		this.maxResult = maxResult;
	}
	public String getQuery() {
		return query;
	}
	public void setQuery(String query) {
		this.query = query;
	}
}
