package com.tarena.util;

/**
 * 页码索引
 */
public class PageIndex {
	/** 开始页索引  */
	private long startIndex;
	/** 结束页索引  */
	private long endIndex;
	
	public PageIndex(long startIndex, long endIndex) {
		this.startIndex = startIndex;
		this.endIndex = endIndex;
	}

	public long getStartIndex() {
		return startIndex;
	}

	public void setStartIndex(long startIndex) {
		this.startIndex = startIndex;
	}

	public long getEndIndex() {
		return endIndex;
	}

	public void setEndIndex(long endIndex) {
		this.endIndex = endIndex;
	}
}
