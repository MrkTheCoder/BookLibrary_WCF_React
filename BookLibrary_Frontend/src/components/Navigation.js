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

function Navigation({ Showcategories, showItems }) {
  const dispatch = useDispatch();
  const categoriesFromState = useSelector((state) => state.categories);
  const { categories, loading, error: categoryError } = categoriesFromState;

  const filtersFromState = useSelector((state) => state.filters);
  const {
    filters,
    success: filterSuccess,
    error: filterError,
  } = filtersFromState;

  const [CATEGORY, setCATEGORY] = useState(filters ? filters.category : null);
  const [ITEM, setITEM] = useState(filters ? filters.item : 10);

  const itemsList = [1, 10, 20, 30, 40, 50];

  useEffect(() => {
    categories.length == 0 && dispatch(categoryList());
  }, [dispatch, CATEGORY]);
  const history = useNavigate();
  const addFilterHandler = () => {
    ITEM && history(`?item=${ITEM}`);
    CATEGORY && history(`?category=${CATEGORY}`);

    dispatch(
      addFilters({
        category: CATEGORY,
        item: Number(ITEM),
      })
    );
  };
  const clearFiltersHandler = () => {
    dispatch({ type: RESET_FILTERS });
    setCATEGORY();
    history("/");
  };
  return (
    <div>
      <Nav variant="pills" className="bg-light navigation">
        <div className="navigationItems">
          {Showcategories && categoryError ? (
            <div></div>
          ) : (
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

          <Nav.Item>
            <NavDropdown title={ITEM != null || 0 ? ITEM : "Items"}>
              {itemsList.map((item) => (
                <NavDropdown.Item key={item} onClick={(e) => setITEM(item)}>
                  <div>{item}</div>
                </NavDropdown.Item>
              ))}
            </NavDropdown>
          </Nav.Item>
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
