package com.tarena.service.employee;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.tarena.bean.employee.Employee;
import com.tarena.service.base.AbstractDAO;

@Service
@Transactional
public class EmployeeServiceImpl extends AbstractDAO<Employee> implements
		IEmployeeService {
}
