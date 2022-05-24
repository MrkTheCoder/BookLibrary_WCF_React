import React, { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { Nav, NavDropdown, Button } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { categoryList } from "../actions/categoryActions";
import { addFilters } from "../actions/categoryActions";
import { useDispatch, useSelector } from "react-redux";
import Message from "../components/Message";
import Loader from "../components/Loader";
import "./Navigation.css";
import { RESET_FILTERS } from "../constants/categoryConstants";

function Navigation({ Showcategories, showItems, redirect }) {
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
  const [tempFilter, setTempFilter] = useState(filters ? filters : {});
  const [searchParams, setSearchParams] = useSearchParams();

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
        ...tempFilter,
      })
    );
  };
  const clearFiltersHandler = () => {
    dispatch({ type: RESET_FILTERS });
    setCATEGORY();
    history(redirect ? redirect : "/");
  };

  const selectAllCategiries = (e) => {
    setCATEGORY(null);

    if (tempFilter && tempFilter.category) {
      delete tempFilter["category"];

      setCATEGORY(null);
      addFilterHandler();
    }
  };

  const addHandler = (e, value, key) => {
    if (key == "category") {
      setCATEGORY(value);
    }
    if (key == "item") {
      setITEM(value);
    }

    if (tempFilter) {
      tempFilter[key] = value;
    }
  };
  return (
    <div>
      <Nav variant="pills" className="bg-light navigation">
        <div className="navigationItems">
          {categoryError ? (
            <div></div>
          ) : Showcategories ? (
            <Nav.Item>
              <NavDropdown
                title={CATEGORY ? `Category:${CATEGORY}` : "Category:All"}
                id="nav-dropdown"
                data-testid="category_dropdown_button"
              >
                <NavDropdown.Item>
                  <div
                    value={null}
                    onClick={(e) => selectAllCategiries(e)}
                    className="categoryList"
                  >
                    All
                  </div>
                </NavDropdown.Item>
                {categories.map((cat) => (
                  <NavDropdown.Item key={cat.Name}>
                    <div
                      className="categoryList"
                      onClick={(e) => addHandler(e, cat.Name, "category")}
                    >
                      <span data-testid="categoryFromApi">{cat.Name}</span>
                      <div className="roundInfo">{cat.BooksInCategory}</div>
                    </div>
                  </NavDropdown.Item>
                ))}
              </NavDropdown>
            </Nav.Item>
          ) : (
            <div></div>
          )}
          {showItems && (
            <Nav.Item>
              <NavDropdown title={`Items:${ITEM}`}>
                {itemsList.map((item) => (
                  <NavDropdown.Item
                    key={item}
                    onClick={(e) => addHandler(e, item, "item")}
                  >
                    <div>{item}</div>
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
            <div className="noDecoration">Apply filters</div>
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
