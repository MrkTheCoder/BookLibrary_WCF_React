import React from "react";
import { Nav, NavDropdown } from "react-bootstrap";

function Navigation() {
  return (
    <div>
      <Nav variant="pills" className="bg-light">
        <Nav.Item>
          <NavDropdown title="Categories" id="nav-dropdown">
            <NavDropdown.Item eventKey="4.1">Action</NavDropdown.Item>
            <NavDropdown.Item eventKey="4.2">Another action</NavDropdown.Item>
            <NavDropdown.Item eventKey="4.3">
              Something else here
            </NavDropdown.Item>
            <NavDropdown.Divider />
            <NavDropdown.Item eventKey="4.4">Separated link</NavDropdown.Item>
          </NavDropdown>
        </Nav.Item>
      </Nav>
    </div>
  );
}

export default Navigation;
