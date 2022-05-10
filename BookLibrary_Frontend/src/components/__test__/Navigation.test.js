/* eslint-disable testing-library/render-result-naming-convention */
import React from "react";
import Navigation from "../Navigation";
import { render, fireEvent, waitFor, screen } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import store from "../../store";
import nock from "nock";

const categories = [
  {
    BooksInCategory: 1,
    Name: "1",
  },
  {
    BooksInCategory: 2,
    Name: "2",
  },
  {
    BooksInCategory: 3,
    Name: "3",
  },
  {
    BooksInCategory: 4,
    Name: "4",
  },
];
const renderEL = (cat, item) => {
  const element = render(
    <Provider store={store}>
      <BrowserRouter>
        <Navigation Showcategories />
      </BrowserRouter>
    </Provider>
  );
  return element;
};
describe("Navigation load", () => {
  test("it shows categories", async () => {
    render(
      <Provider store={store}>
        <BrowserRouter>
          <Navigation Showcategories />
        </BrowserRouter>
      </Provider>
    );

    const catMenuButton = screen.getByRole("button", { name: /category:all/i });
    fireEvent.click(catMenuButton);

    const categories = await screen.findAllByTestId(/categoryFromApi/);
    await expect(categories).toHaveLength(4);
  });
});
