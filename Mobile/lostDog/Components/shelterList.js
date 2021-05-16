import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity ,ScrollView,SafeAreaView,FlatList} from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import ShelterListItem from './Helpers/ShelterListItem';

const {width, height} = Dimensions.get("screen")

export default class shelterList extends React.Component {c
  state={
    info: " normal ",
    image: null,
    ShelterList: [],
  }

  getShelterList = ()=>{
    this.props.Navi.RunOnBackend("getShelterList",null).then((responseData)=>{
      console.log(responseData)
      this.setState({ShelterList: responseData});
      console.log("succes list of shelters is ready")
    }).catch((x)=>
        console.log("getShelterList Error" + (x))
      )
  }

  constructor(props) {
    super(props);
    this.getShelterList();
   }
   
   shelterSelected=(item)=>{
    //console.log("Dog is selected " + item.id);
    this.props.Navi.swtichPage(10,item);
  }

  render(){
    return(
        <View style={styles.content}>
           <FlatList
            data={this.state.ShelterList.length>0 ? this.state.ShelterList : []}
            renderItem={({item}) => <ShelterListItem item={item} shelterSelected={this.shelterSelected}/>}
            keyExtractor={(item) => item.id.toString()}
           />
        </View>
  )
  }
}

const styles = StyleSheet.create({
  content: {
    marginTop:30,
    margin: 15,
    height: '90%',
    alignSelf: 'center',
    justifyContent: 'center',
  }
});