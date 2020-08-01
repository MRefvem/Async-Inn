select ANU.UserName, ANU.FirstName, ANU.LastName, ANR.Name as "Role Name"
from AspNetUsers ANU
join AspNetUserRoles ANUR on ANU.Id = ANUR.UserId
join AspNetRoles ANR on ANUR.RoleId = ANR.Id
order by ANU.LastName, ANU.FirstName