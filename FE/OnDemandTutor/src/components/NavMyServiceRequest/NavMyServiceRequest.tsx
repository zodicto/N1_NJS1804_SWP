import { useState } from "react";
import { Link, useLocation } from "react-router-dom";
import { path } from "../../constant/path";

function NavMyServiceRequest() {
    const location = useLocation()
    const [selected, setSelected] = useState(location.pathname)
    return ( <>
        <div className='flex justify-around p-5 border-2 shadow-xl mb-5'>
            <Link to={path.myClassLayout}>
                <div
                onClick={() => setSelected(path.myClassLayout)}
                className={`text-base font-bold cursor-pointer py-1 relative after:absolute after:bottom-0 after:left-0 after:bg-pink-600 after:h-0.5 ${
                    selected === path.myClassLayout
                    ? 'text-pink-600 after:w-full'
                    : 'hover:text-pink-600 after:w-0 hover:after:w-full'
                } after:transition-all after:ease-in-out after:duration-100`}
                >
                Xem danh sách yêu cầu 
                </div>
            </Link>
            {/* <Link to={path.home}>
                <div
                //onClick={() => setSelected(path.RequestStudentCurrent)}
                className={`text-base font-bold cursor-pointer py-1 relative after:absolute after:bottom-0 after:left-0 after:bg-pink-600 after:h-0.5
                //  ${selected === path.RequestStudentCurrent ? 'text-pink-600 after:w-full' : 'hover:text-pink-600 after:w-0 hover:after:w-full'} after:transition-all after:ease-in-out after:duration-100`}
                >
                Xem danh sách bị từ chối
                </div>
            </Link> */}
            <Link to={path.myService}>
                <div
                onClick={() => setSelected(path.myService)}
                className={`text-base font-bold cursor-pointer py-1 relative after:absolute after:bottom-0 after:left-0 after:bg-pink-600 after:h-0.5 ${
                    selected === path.myService
                    ? 'text-pink-600 after:w-full'
                    : 'hover:text-pink-600 after:w-0 hover:after:w-full'
                } after:transition-all after:ease-in-out after:duration-100`}
                >
                Xem danh sách dịch vụ đã đăng kí
                </div>
            </Link>
            </div>
    </> );
}

export default NavMyServiceRequest;