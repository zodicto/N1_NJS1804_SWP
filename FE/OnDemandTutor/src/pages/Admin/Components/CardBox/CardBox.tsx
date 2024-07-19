import { colorsText } from "../../../../utils/color";
import Icon from "./Icon";
import NumberDynamic from "./NumberDynamic";

interface Props {
    number: number;
    icon: string;
    label: string;
    //persent: string;
    //trendType: string;
    iconColor: string;
    numberPrefix?: string;
    numberSuffix?: string;
}

export default function CardBox(props: Props): JSX.Element {
    return (
        <>
            <div className="shadow-md bg-slate-50 rounded-md p-3 w-2/5">
                <div className="flex justify-between h-[120px]">
                    <div className="flex flex-col text-left w-full gap-4  ">
                        <div className="text-lg leading-tight text-gray-500 dark:text-slate-400">
                            {props.label}
                        </div>
                        <div className="text-xl items-center leading-tight font-semibold flex justify-between">
                            <NumberDynamic
                                value={props.number}
                                prefix={props.numberPrefix}
                                suffix={props.numberSuffix}
                            />
                            {props.icon && (
                                <Icon
                                    path={props.icon}
                                    size="48"
                                    w=""
                                    h="h-16"
                                    className={colorsText[props.iconColor]}
                                />
                            )}
                        </div>
                    </div>
                    
                </div>
            </div>
        </>
    );
}