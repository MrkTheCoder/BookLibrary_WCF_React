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
            <Nav.Link href="#features">Books</Nav.Link>

            <NavDropdown
              title={`Ali batashi${(<i className="fas fa-solid"></i>)}`}
              id="collasible-nav-dropdown"
            >
              <LinkContainer to={"/user"}>
                <NavDropdown.Item>Profile</NavDropdown.Item>
              </LinkContainer>
              <LinkContainer to={"/userbooks"}>
                <NavDropdown.Item>Books</NavDropdown.Item>
              </LinkContainer>
              <LinkContainer to={"/admin/borrowers"}>
                <NavDropdown.Item>Borrowers list</NavDropdown.Item>
              </LinkContainer>
              <NavDropdown.Divider />
              <LinkContainer to={"/logout"}>
                <NavDropdown.Item>Logout</NavDropdown.Item>
              </LinkContainer>
            </NavDropdown>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}
