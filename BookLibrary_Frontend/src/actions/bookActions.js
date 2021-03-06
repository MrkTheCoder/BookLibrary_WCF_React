import axios from "axios";
import { useSelector } from "react-redux";
import {
  BOOK_LIST_REQUEST,
  BOOK_LIST_SUCCESS,
  BOOK_LIST_FAIL,
  //
  BOOK_DETAILS_REQUEST,
  BOOK_DETAILS_SUCCESS,
  BOOK_DETAILS_FAIL,
} from "../constants/bookConstants";
import etagHandler from "../functions/etagHandler";
export const listBooks = (allFilters) => async (dispatch) => {
  try {
    dispatch({ type: BOOK_LIST_REQUEST });
    if (allFilters == null) {
      allFilters = { item: 10 };
    }

    const { data, headers } = await axios.get(
      `http://localhost:51202/api/BookManager/books`,

      { params: allFilters }
    );
    // checking for etag
    //console.log(headers, "Recived data");
    //const newData = await etagHandler(headers, data);

    dispatch({
      type: BOOK_LIST_SUCCESS,
      payload: data,
      headers: headers,
    });
  } catch (error) {
    dispatch({
      type: BOOK_LIST_FAIL,
      payload:
        error.response && error.response.data.Message
          ? error.response.data.Message
          : error.message,
    });
  }
};

export const bookDetailsAction = (Isbn) => async (dispatch) => {
  try {
    dispatch({ type: BOOK_DETAILS_REQUEST });
    const { data } = await axios.get(
      `http://localhost:51202/api/BookManager/books/${Isbn}`
    );
    dispatch({ type: BOOK_DETAILS_SUCCESS, payload: data });
  } catch (error) {
    dispatch({
      type: BOOK_DETAILS_FAIL,
      payload:
        error.response && error.response.data.Message
          ? error.response.data.Message
          : error.message,
    });
  }
};
