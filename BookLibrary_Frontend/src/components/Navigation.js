import React from "react";
import { Nav, NavDropdown } from "react-bootstrap";
import Categories from "./Categories";

function Navigation({ categories }) {
  return (
    <div>
      <Nav variant="pills" className="bg-light">
        {categories && <Categories />}
      </Nav>
    </div>
  );
}

export default Navigation;
