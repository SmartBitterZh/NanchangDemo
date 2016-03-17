package com.tarena.action.employee;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import javax.annotation.Resource;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.struts.action.Action;
import org.apache.struts.action.ActionForm;
import org.apache.struts.action.ActionForward;
import org.apache.struts.action.ActionMapping;
import org.springframework.stereotype.Controller;

import com.tarena.bean.employee.Employee;
import com.tarena.formbean.employee.EmployeeForm;
import com.tarena.service.employee.IEmployeeService;
import com.tarena.util.PageView;

@Controller("/emp/list")
public class EmployeeAction extends Action {
	
	@Resource(name="employeeServiceImpl")
	private IEmployeeService service;

	@Override
	public ActionForward execute(ActionMapping mapping, ActionForm form,
			HttpServletRequest request, HttpServletResponse response)
			throws Exception {
		EmployeeForm formbean = (EmployeeForm) form;
		PageView<Employee> pageView = new PageView<Employee>(formbean.getPage(),formbean.getMaxResult());
		int firstResult = (pageView.getCurrentPage()-1) * pageView.getMaxResult();
		StringBuffer wherehql = null;
		List<Object> params = null;
		if("true".equals(formbean.getQuery())){
			wherehql = new StringBuffer();
			params = new ArrayList<Object>();
			String name = formbean.getName();
			String address = formbean.getAddress();
			String phone = formbean.getPhone();
			if(name!=null && name.length()>0){
				wherehql.append(" o.name like ? ");
				params.add("%"+ name +"%");
			}
			if(address!=null && address.length()>0){
				if(wherehql.length()>0) wherehql.append(" and ");
				wherehql.append(" o.address like ? ");
				params.add("%"+ address +"%");
			}
			if(phone!=null && phone.length()>0){
				if(wherehql.length()>0) wherehql.append(" and ");
				wherehql.append(" o.phone like ? ");
				params.add("%"+ phone +"%");
			}
		}
		Map<String, String> orderby = new LinkedHashMap<String, String>();
		orderby.put("id", "desc");
		orderby.put("name", "asc");
		pageView.setQueryResult(service.getPageData(firstResult, pageView.getMaxResult(),(wherehql==null||wherehql.length()<1) ? null : wherehql.toString(),(params==null||params.size()<1) ? null : params.toArray(), orderby));
		request.setAttribute("pageView", pageView);
		return mapping.findForward("list");
	}
}
