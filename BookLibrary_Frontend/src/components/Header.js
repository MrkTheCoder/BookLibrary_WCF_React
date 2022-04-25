import React from "react";
import { Navbar, Container, NavDropdown, Nav } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { RESET_FILTERS } from "../constants/categoryConstants";
import { useDispatch } from "react-redux";
export default function Header() {
  const dispatch = useDispatch();
  const resetHandler = () => {
    dispatch({ type: RESET_FILTERS });
  };
  return (
    <Navbar
      className="navConfig"
      collapseOnSelect
      expand="lg"
      bg="dark"
      variant="dark"
    >
      <Container>
        <LinkContainer onClick={(e) => resetHandler()} to={"/"}>
          <Navbar.Brand>Library</Navbar.Brand>
        </LinkContainer>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
          <Nav className="me-auto">
            <NavDropdown title={`Manage`} id="collasible-nav-dropdown">
              <LinkContainer to={"/admin/borrowers"}>
                <NavDropdown.Item>Borrowers list</NavDropdown.Item>
              </LinkContainer>
            </NavDropdown>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}
