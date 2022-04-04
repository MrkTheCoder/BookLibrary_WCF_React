import React, { useEffect } from "react";
import { Nav, NavDropdown, Badge } from "react-bootstrap";
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
  return (
    <div>
      {loading ? (
        <Loader />
      ) : error ? (
        <Message variant="danget">{error}</Message>
      ) : (
        <Nav.Item>
          <NavDropdown title="Categories" id="nav-dropdown">
            {console.log(categories)}
            {categories.map((cat) => (
              <NavDropdown.Item key={cat.Name}>
                <div className="categoryList">
                  <div>{cat.Name}</div>
                  <div className="roundInfo">{cat.BooksInCategory}</div>
                </div>
              </NavDropdown.Item>
            ))}
          </NavDropdown>
        </Nav.Item>
      )}
    </div>
  );
}

export default Categories;
