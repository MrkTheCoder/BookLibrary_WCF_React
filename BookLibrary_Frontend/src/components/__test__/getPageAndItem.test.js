import getPageAndItem from "../getPageAndItem";



describe('pageAndItemTest', () =>{
    test('no data shold return 0.',() => {
        expect(getPageAndItem()).toBe(0)
    })
    
    test('p=1&i=10 as prevPage should return p=2&i=10',() => {

        const {page,item} = getPageAndItem('/?page=1&item=10', 'prevPage')
        expect(page).toBe(2)
        expect(item).toBe('10')
    })
    test('p=2&i=432 as nextPage should return p=1&i=432', () => {
        const {page,item} = getPageAndItem('/?page=2&item=432', 'nextPage')
        expect(page).toBe(1)
        expect(item).toBe('432')
    })
})
