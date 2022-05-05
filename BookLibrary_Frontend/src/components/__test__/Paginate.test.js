import React from "react";
import Paginate from "../Paginate";
import { render,fireEvent } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import { MemoryRouter,BrowserRouter } from "react-router-dom";


const renderEL = (data) =>{
  const element = render(
    <BrowserRouter>
      <Paginate totalItems={data.totalItems} nextPage={data.nextPage} />
    </BrowserRouter>
  );
  return element
}
test("Element renders without errors", () => {
  const data = {
    totalItems: 21,
    nextPage: "?page=2&item=10",
  };
  const { getByTestId } = renderEL(data)

});

describe('we are on the first page,there are more than 3 pages' , () =>{
  const data ={
    totalItems: 50,
    nextPage: "?page=2&item=10",
  }
  
  test('prev page and first page should be disabled', () => {
    const {getByTestId,getByText} = renderEL(data)
    


    const lastPage = getByText('First')
    const prevPage = getByText('Previous')

    expect(lastPage.className).toBe('visually-hidden')
    expect(prevPage.className).toBe('visually-hidden')

  })

  test('page 1 is shown with correct link', () =>{
    const {getByTestId,getByText} = renderEL(data)
    
   
  
    const currentPAge = getByTestId("firstPageLink");
  
  
  
    expect(currentPAge.closest('a')).toHaveAttribute('href',"/?page=1&item=10");
    
    })
    test('page 2 is  shown with correct link', () =>{
      const {getByTestId,getByText} = renderEL(data)
      
     
    
      
      const secoundPage = getByTestId('1pageAfterLink')
      
    
    
      
      expect(secoundPage.closest('a')).toHaveAttribute('href',"/?page=2&item=10");
      
      })
      test('page 3 is shown with correct link', () =>{
        
        const {getByTestId,getByText} = renderEL(data)
       
      
        
        const thirdPage = getByTestId("2pageAfterLink")
      
      
        
        expect(thirdPage.closest('a')).toHaveAttribute('href',"/?page=3&item=10");
        })
        test('custom page drop down is shown correctly and clicking will open it',() =>{
          const {getByTestId,getByText} = renderEL(data)

          const customButton = getByTestId('threeDots')
          fireEvent.click(customButton)

          const customPageForm = getByTestId('customPageForm')
          const customButtonFromSubmit = getByTestId('customButtonFromSubmit')
          expect(customPageForm).toHaveAttribute('placeholder', 'Enter page number...')
          expect(customButtonFromSubmit.textContent).toBe('GO!')
          

          expect
        })
        test('next page button has the correct link',() => {
          const {getByTestId} = renderEL(data)

          const nextPageButton = getByTestId('nextPageLink')
          expect (nextPageButton).toHaveAttribute('href', '/?page=2&item=10')

        })
        test('last page button has the correct link',() =>{
          const{getByTestId} = renderEL(data)
          const lastPageButton = getByTestId('lastPageLink')
          expect(lastPageButton).toHaveAttribute('href', '/?page=5&item=10')
        })
          
})

describe('we are on the 1st page, there are less than 3 pages', () =>{
  const data ={
    totalItems: 15,
    nextPage: "?page=2&item=10",
  }
  
  test('page 1 button has the right link', () =>{
    const {getByTestId, getByText} = renderEL(data)
    const page1Button = getByTestId('currentPageLink')
    expect(page1Button).toHaveAttribute('href', '/?page=1&item=10')
  })
  test('page 2 button has the right link', ()=>{
    const {getByTestId, getByText} = renderEL(data)
    const page1Button = getByTestId('1pageAfterLink')
    expect(page1Button).toHaveAttribute('href', '/?page=2&item=10')
  })
})

describe('we are in the middle, there are 20 pages', () =>{
  const data={
    totalItems:60,
    prevPage: "?page=2&item=10",
    nextPage: "?page=4&item=10",
    
  }
  test('current page has correct link' ,() =>{
    const {getByTestId, getByText} = renderEL(data)
    const page1Button = getByTestId('currentPageLink')
    expect(page1Button).toHaveAttribute('href', '/?page=3&item=10')
  })
  test('next page has correct link' ,() =>{
    const {getByTestId, getByText} = renderEL(data)
    const page1Button = getByTestId('1pageAfterLink')
    expect(page1Button).toHaveAttribute('href', '/?page=4&item=10')
  })
  test('next page button has correct link' ,() =>{
    const {getByTestId, getByText} = renderEL(data)
    const page1Button = getByTestId('nextPageLink')
    expect(page1Button).toHaveAttribute('href', '/?page=4&item=10')
  })
  test('last page button correct link' ,() =>{
    const {getByTestId, getByText} = renderEL(data)
    const page1Button = getByTestId('lastPageLink')
    expect(page1Button).toHaveAttribute('href', '/?page=6&item=10')
  })
  test('previous page has correct link' ,() =>{
    const {getByTestId, getByText} = renderEL(data)
    const page1Button = getByTestId('prevPageLink')
    expect(page1Button).toHaveAttribute('href', '/?page=2&item=10')
  })
  test('first page has correct link' ,() =>{
    const {getByTestId, getByText} = renderEL(data)
    const page1Button = getByTestId('firstPageLink')
    expect(page1Button).toHaveAttribute('href', '/?page=1&item=10')
  })
  

   
})