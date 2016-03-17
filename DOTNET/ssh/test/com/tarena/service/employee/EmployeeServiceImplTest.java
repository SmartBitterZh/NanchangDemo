package com.tarena.service.employee;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import org.junit.Test;

import com.tarena.bean.employee.Employee;
import com.tarena.util.QueryResult;

public class EmployeeServiceImplTest {
	IEmployeeService service = new EmployeeServiceImpl();

	@Test
	public void save() {
		for (int i = 1; i < 101; i++) {
			Employee emp = new Employee();
			emp.setName("张三" + i);
			emp.setAddress("广州市天河区天河路" + i + "号");
			emp.setPhone("87654321-"+i);
			service.save(emp);
		}
	}
	
	@Test
	public void getPageData(){
		StringBuffer wherehql = new StringBuffer();
		wherehql.append("o.name like ?");
		List<String> params = new ArrayList<String>();
		params.add("%3%");
		Map<String,String> orderby = new LinkedHashMap<String, String>();
		orderby.put("id", "desc");
		QueryResult<Employee> qr = service.getPageData(0,10,wherehql.toString(),params.toArray(),orderby);
		for(Employee emp : qr.getResultList()){
			System.out.println("emp:"+emp.getName());
		}
	}
}
