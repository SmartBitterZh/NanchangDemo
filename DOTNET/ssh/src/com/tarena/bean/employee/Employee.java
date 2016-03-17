package com.tarena.bean.employee;

public class Employee implements java.io.Serializable {

	private static final long serialVersionUID = 1951975215735444438L;
	private Long id;
	private String name;
	private String address;
	private String phone;

	public Employee() {
	}

	public Employee(String name, String address, String phone) {
		this.name = name;
		this.address = address;
		this.phone = phone;
	}

	public Long getId() {
		return this.id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public String getName() {
		return this.name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getAddress() {
		return this.address;
	}

	public void setAddress(String address) {
		this.address = address;
	}

	public String getPhone() {
		return this.phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}
}