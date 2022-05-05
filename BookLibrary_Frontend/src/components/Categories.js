import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Nav, NavDropdown, Badge } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { categoryList } from "../actions/categoryActions";
import { useDispatch, useSelector } from "react-redux";
import Message from "../components/Message";
import Loader from "../components/Loader";
function Categories() {
  const dispatch = useDispatch();
  const categoriesFromState = useSelector((state) => state.categories);
  const { categories, loading, error } = categoriesFromState;

  useEffect(() => {
    dispatch(categoryList());
  }, [dispatch]);
  const history = useNavigate();
  const navHandler = (cat) => {
    history(cat);
  };
  return (
    <div>
      {loading ? (
        <Loader />
      ) : error ? (
        <Message variant="danget">{error}</Message>
      ) : (
        <Nav.Item>
          <NavDropdown title="Categories" id="nav-dropdown">
            {categories.map((cat) => (
              <NavDropdown.Item key={cat.Name}>
                <LinkContainer to={`?category=${cat.Name}&item=10`}>
                  <div className="categoryList">
                    <span>{cat.Name}</span>
                    <div className="roundInfo">{cat.BooksInCategory}</div>
                  </div>
                </LinkContainer>
              </NavDropdown.Item>
            ))}
          </NavDropdown>
        </Nav.Item>
      )}
    </div>
  );
}

export default Categories;
