package com.tarena.action.employee;

import javax.annotation.Resource;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.struts.action.ActionForm;
import org.apache.struts.action.ActionForward;
import org.apache.struts.action.ActionMapping;
import org.apache.struts.actions.DispatchAction;
import org.springframework.stereotype.Controller;

import com.tarena.bean.employee.Employee;
import com.tarena.formbean.employee.EmployeeForm;
import com.tarena.service.employee.IEmployeeService;
import com.tarena.util.ReadResourceMapping;

@Controller("/emp/manage")
public class EmployeeManageAction extends DispatchAction{
	
	@Resource(name="employeeServiceImpl")
	private IEmployeeService service;
	
	/** 添加雇员界面 */
	public ActionForward addUI(ActionMapping mapping, ActionForm form,
			HttpServletRequest request, HttpServletResponse response) {
		this.saveToken(request);
		return mapping.findForward("add");
	}
	
	/** 添加雇员 */
	public ActionForward add(ActionMapping mapping, ActionForm form,
			HttpServletRequest request, HttpServletResponse response) {
		if(this.isTokenValid(request,true)){
			EmployeeForm formbean = (EmployeeForm) form;
			String name = formbean.getName();
			String address = formbean.getAddress();
			String phone = formbean.getPhone();
			Employee emp = new Employee(name,address,phone);
			service.save(emp);
			request.setAttribute("message", "添加成功");
		}else{
			request.setAttribute("message", "请勿重复提交");
		}
		request.setAttribute("url", ReadResourceMapping.get("emp.list"));
		return mapping.findForward("message");
	}
	
	/** 修改雇员界面 */
	public ActionForward editUI(ActionMapping mapping, ActionForm form,
			HttpServletRequest request, HttpServletResponse response) {
		EmployeeForm formbean = (EmployeeForm) form;
		Employee emp = service.find(formbean.getId());
		formbean.setName(emp.getName());
		formbean.setAddress(emp.getAddress());
		formbean.setPhone(emp.getPhone());
		return mapping.findForward("edit");
	}
	
	/** 修改雇员 */
	public ActionForward edit(ActionMapping mapping, ActionForm form,
			HttpServletRequest request, HttpServletResponse response) {
		EmployeeForm formbean = (EmployeeForm) form;
		Employee emp = service.find(formbean.getId());
		emp.setName(formbean.getName());
		emp.setAddress(formbean.getAddress());
		emp.setPhone(formbean.getPhone());
		service.update(emp);
		request.setAttribute("message", "修改成功");
		request.setAttribute("url", ReadResourceMapping.get("emp.list"));
		return mapping.findForward("message");
	}
	
	/** 删除雇员 */
	public ActionForward delete(ActionMapping mapping, ActionForm form,
			HttpServletRequest request, HttpServletResponse response) {
		EmployeeForm formbean = (EmployeeForm) form;
		Long id = formbean.getId();
		Long[] ids = formbean.getIds();
		if(id!=null && id.longValue()>0){
			service.delete(id);
		}else if(ids!=null && ids.length>0){
			service.delete(ids);
		}
		request.setAttribute("message", "删除成功");
		request.setAttribute("url", ReadResourceMapping.get("emp.list"));
		return mapping.findForward("message");
	}
}
