import { Modal, Nav, Tab } from "react-bootstrap";
import {
  CustomerResponseDto,
  TeamMemberResponseDto,
  TeamResponseDto,
  UserInfoDto
} from "../../../../api/client";
import DeleteTeam from "../delete/DeleteTeam";
import EditTeam from "./EditTeam";

interface Props {
  show: boolean;
  onHide: () => void;
  team: TeamResponseDto;
  customers: CustomerResponseDto[];
  teammembers: TeamMemberResponseDto[];
  users: UserInfoDto[];
}

const EditTeamModal = ({ team, teammembers, users, customers, show, onHide }: Props) => {
  return (
    <Modal show={show} id='editTeam' size='lg' onHide={() => onHide()}>
      <Modal.Header closeButton onHide={() => onHide()}>
        <Modal.Title>{team.name}</Modal.Title>
      </Modal.Header>
      <Modal.Body className='px-3 py-2 mb-2'>
        <Tab.Container defaultActiveKey='first'>
          <Nav variant='pills' className='gap-2 flex-row mb-4 align-items-center'>
            <Nav.Item>
              <Nav.Link eventKey='first'>Översikt</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey='second'>Inställningar</Nav.Link>
            </Nav.Item>
            <DeleteTeam team={team} />
          </Nav>
          <Tab.Content>
            <Tab.Pane eventKey='first'>
              <div className='mb-2'>
                <div className='fs-5'>Kunder</div>
                {customers
                  .filter((x) => x.teamId === team.id)
                  .map((customer) => {
                    return <div key={customer.id}>{customer.name}</div>;
                  })}
              </div>
              <div>
                <div className='fs-5'>Teammedlemmar</div>
                {teammembers
                  .filter((x) => x.teamId === team.id)
                  .map((teammember) => {
                    return (
                      <div key={teammember.userId}>
                        {users?.find((user) => teammember.userId === user.userId)?.displayName}
                      </div>
                    );
                  })}
              </div>
            </Tab.Pane>
            <Tab.Pane eventKey='second'>
              <EditTeam users={users} team={team} teammembers={teammembers} />
            </Tab.Pane>
          </Tab.Content>
        </Tab.Container>
      </Modal.Body>
    </Modal>
  );
};

export default EditTeamModal;
