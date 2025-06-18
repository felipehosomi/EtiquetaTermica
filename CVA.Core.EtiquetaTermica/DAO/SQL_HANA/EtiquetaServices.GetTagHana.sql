select "Code"
      ,"Name"
	  ,"U_descricao"
	  ,"U_script" as "U_script"
	  ,"U_linguagem"
	  ,"U_procedure"
	  ,"U_local"
	  ,"U_procpesq"
  from "@CVA_ETIQUETA"
 where "Canceled" = 'N'
   and "Code" = '{0}'
 order by 1	