package com.tarena.dwr;

import java.util.List;

import javax.annotation.Resource;

import org.directwebremoting.annotations.Param;
import org.directwebremoting.annotations.RemoteMethod;
import org.directwebremoting.annotations.RemoteProxy;
import org.directwebremoting.spring.SpringCreator;
import org.springframework.stereotype.Component;

import com.tarena.bean.employee.Employee;
import com.tarena.service.employee.IEmployeeService;

@Component
@RemoteProxy(creator=SpringCreator.class,
		creatorParams={@Param(name="beanName",value="testEmployee")})
public class TestEmployee {
	
	@Resource(name="employeeServiceImpl")
	private IEmployeeService service;

	@RemoteMethod
	public String testName(String name){
		if(name==null || "".equals(name.trim()))
			return "　请输入雇员姓名！";
		List<Employee> list = service.findByProperty("name", name);
		if(list!=null && list.size()>0)
			return " 此雇员已存在，不必重复添加！";
		return " √";
	}
}
