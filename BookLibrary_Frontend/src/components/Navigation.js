import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Nav, NavDropdown, Button } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { categoryList } from "../actions/categoryActions";
import { addFilters } from "../actions/categoryActions";
import { useDispatch, useSelector } from "react-redux";
import Message from "../components/Message";
import Loader from "../components/Loader";
import "./style.css";
import { RESET_FILTERS } from "../constants/categoryConstants";

function Navigation({ Showcategories }) {
  const dispatch = useDispatch();
  const categoriesFromState = useSelector((state) => state.categories);
  const { categories, loading, error } = categoriesFromState;

  const filtersFromState = useSelector((state) => state.filters);
  const {
    filters,
    success: filterSuccess,
    error: filterError,
  } = filtersFromState;

  const [CATEGORY, setCATEGORY] = useState();

  useEffect(() => {
    categories.length == 0 && dispatch(categoryList());
    console.log(CATEGORY);
  }, [dispatch, CATEGORY]);
  const history = useNavigate();
  const addFilterHandler = () => {
    dispatch(
      addFilters({
        category: CATEGORY,
      })
    );
  };
  const clearFiltersHandler = () => {
    dispatch({ type: RESET_FILTERS });
    setCATEGORY();
  };
  return (
    <div>
      <Nav variant="pills" className="bg-light navigation">
        <div className="navigationItems">
          {Showcategories && (
            <Nav.Item>
              <NavDropdown
                title={CATEGORY ? CATEGORY : "Category"}
                id="nav-dropdown"
              >
                {categories.map((cat) => (
                  <NavDropdown.Item key={cat.Name}>
                    <div
                      className="categoryList"
                      onClick={(e) => setCATEGORY(cat.Name)}
                    >
                      <span>{cat.Name}</span>
                      <div className="roundInfo">{cat.BooksInCategory}</div>
                    </div>
                  </NavDropdown.Item>
                ))}
              </NavDropdown>
            </Nav.Item>
          )}
        </div>
        <div>
          <Button
            className="navigationSubmit noDecoration "
            type="submit"
            onClick={(e) => addFilterHandler()}
          >
            <div className="noDecoration">Add filters</div>
          </Button>
          <Button
            className="navigationSubmit noDecoration "
            type="submit"
            onClick={(e) => clearFiltersHandler()}
          >
            <div className="noDecoration">Reset filters</div>
          </Button>
        </div>
      </Nav>
    </div>
  );
}

export default Navigation;
