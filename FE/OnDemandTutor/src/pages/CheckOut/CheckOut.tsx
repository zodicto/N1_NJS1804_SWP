import React, { useState } from "react"

export default function CheckOut (){
    const [selected, setSelected] = useState<'select1' | 'select2' | null>(null);

    const handleSelect = (type: 'select1' | 'select2') => {
    setSelected(type);
    };

    const isSelected = (type: 'select1' | 'select2') => {
        return selected === type;
  };
    return(
        <>
            <div className="flex m-7">
                <div className="container grid grid-cols-12 gap-4 ">
                    <div className=" col-span-8 p-5 rounded-md border-2 border-gray-600 drop-shadow-lg shadow-md">{/* left rent*/}
                    <div className="flex jus">
                        <div className='w-32 h-32 mr-2 flex-shink-0'>
                        {/*  avataer */}
                            <img
                            className='w-full h-full object-cover'
                            src='https://down-vn.img.susercontent.com/file/vn-11134226-7r98o-luqbzvje8weqe3_tn'
                            />
                        </div>
                        <div className="ml-3 flex flex-col items-start space-y-2">
                            <div className="">Ngô Quang Phước Thành</div>
                            <div className="">21 Tuổi</div>
                            <div className="">4 sao</div>
                        </div>
                    </div>
                    <div className="flex justify-center m-2">
                        <div
                            className={`p-2 pl-10 cursor-pointer ${
                            isSelected('select1') ? 'bg-gray-500' : 'bg-gray-300'
                            } hover:bg-gray-300 active:bg-gray-500 focus-visible:bg-gray-500`}
                            onClick={() => handleSelect('select1')}
                        >
                            Select 1
                        </div>
                        <div
                            className={`p-2 pr-10 cursor-pointer ${
                            isSelected('select2') ? 'bg-gray-500' : 'bg-gray-300'
                            } hover:bg-gray-300 active:bg-gray-500 focus-visible:bg-gray-500`}
                            onClick={() => handleSelect('select2')}
                        >
                            Select 2
                        </div>
                    </div>
                    {
                        selected === 'select1' ? (
                            <div>
                                <div>Thời gian diễn ra phiên thuê</div>
                                <div>
                                    <div>Thông tin Bill</div>
                                    <div>Thuê 30 phút</div>
                                    <div>Tổng:60.000 đ</div>
                                </div>
                            </div>
                        ) : (
                            <div className="p-4 pr-24 pl-24 gap-6">
                                <div>Thời gian diễn ra phiên thuê</div><br />
                                
                                <div>
                                    <div>Thông tin Bill</div>
                                    <div className="flex justify-between"> 
                                        <div>
                                        Thuê:
                                        </div>
                                        <div>60 phút</div>
                                    </div>
                                    <div className="flex justify-between"> 
                                        <div>Tổng: </div>
                                        <div>100.000 đ</div>
                                        </div>
                                </div>
                                
                            </div>
                        )
                    }
                    
                    
                    </div>
                    <div className="col-span-4 ">
                        <div className="bg-white border border-gray-600 hover:bg-slate-400 cursor-pointer p-4 rounded-md drop-shadow-lg shadow-md">Xác nhận thanh toán xxx$</div>
                    </div>
                </div>
                
            </div>
        </>

    )

}