select "Code"
      ,"Name"
	  ,"U_descricao"
	  ,convert(varchar(8000), U_script) as U_script
	  ,"U_linguagem"
	  ,"U_procedure"
	  ,"U_local"
	  ,"U_procpesq"
  from [@cvaetiqueta]
 where canceled = 'N'
   and code = '{0}'
 order by 1	