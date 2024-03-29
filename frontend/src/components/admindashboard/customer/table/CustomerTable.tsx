import { Table } from "react-bootstrap";
import {
  AreaResponseDto,
  ChoreResponseDto,
  CustomerChoreResponseDto,
  CustomerResponseDto,
  Periodic,
  TeamMemberResponseDto,
  TeamResponseDto,
} from "../../../../api/client";

import CustomerRow from "./CustomerRow";

interface Props {
  areas: AreaResponseDto[];
  chores: ChoreResponseDto[];
  customers: CustomerResponseDto[];
  customerchores: CustomerChoreResponseDto[];
  teams: TeamResponseDto[];
  teammembers: TeamMemberResponseDto[];
  periodics: Periodic[];
  search: string;
}

const CustomerTable = ({
  areas,
  chores,
  customers,
  customerchores,
  periodics,
  search,
  teams,
  teammembers,
}: Props) => {
  return (
    <Table hover striped>
      <thead>
        <tr>
          <th className='text-uppercase fs-7'>Kundnamn</th>
          <th className='text-uppercase fs-7'>Klara sysslor</th>
          <th className='text-uppercase fs-7'>Påbörjade sysslor</th>
          <th className='text-uppercase fs-7'>Ej påbörjade sysslor</th>
          <th className='text-uppercase fs-7'>Tilldelat team</th>
          <th className='text-uppercase fs-7'>Alternativ</th>
        </tr>
      </thead>
      <tbody>
        {customers
          ?.filter((customer) => customer.name?.toLowerCase().includes(search.toLowerCase()))
          .map((customer) => (
            <CustomerRow
              key={customer.id}
              areas={areas}
              customer={customer}
              teams={teams}
              teammembers={teammembers}
              customerchores={customerchores}
              periodics={periodics}
              chores={chores}
            />
          ))}
      </tbody>
    </Table>
  );
};

export default CustomerTable;
