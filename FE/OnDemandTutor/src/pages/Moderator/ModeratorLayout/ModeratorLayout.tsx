

import { Outlet } from 'react-router-dom'
import NavMod from '../Components/NavMod'



export default function ModeratorLayout() {
  return (
    <div className='grid grid-cols-12'>
      <div className='col-span-2 shadow-2xl border-r-4 rounded-r-lg h-[760px]'><NavMod/></div>
      <div className='col-span-10  h-[730px] m-7 ml-12'><Outlet/></div>
    </div>
  )
}