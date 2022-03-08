import React from "react";
import Bookcard from "../Bookcard";
import { render, fireEvent, cleanup } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import { act } from "react-dom/test-utils";
import { Link, MemoryRouter as Router } from "react-router-dom";

let getByTestId;
beforeEach(() => {
  const component = render(<Bookcard />);
  getByTestId = component.getByTestId;
});

test("book data returns an error without props", () => {
  const cardEl = getByTestId("noData");

  expect(cardEl.textContent).toBe("Error 500");
});

test("card shows correct date", () => {
  const bookDetails = { name: "book", id: "1" };
  const wrapper = shallow(<Bookcard.WrappedComponent book={bookDetails} />);
  const { getByTestId } = wrapper;

  titleEl = getByTestId("title");
  subtitleEl = getByTestId("subTitle");

  expect(titleEl.textContent).toBe("book");
  expect(subTitleEl.textContent).toBe("1");
});
